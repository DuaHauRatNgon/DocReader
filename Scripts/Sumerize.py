import sys
import re
from transformers import pipeline, AutoTokenizer
import torch

def preprocess_text(text):
    """Tiền xử lý văn bản để cải thiện chất lượng tóm tắt"""
    # Loại bỏ ký tự đặc biệt và dấu xuống dòng thừa
    text = re.sub(r'\n+', ' ', text)
    text = re.sub(r'\s+', ' ', text)
    text = text.strip()
    
    # Loại bỏ các ký tự không cần thiết
    text = re.sub(r'[^\w\s.,!?;:()\-"\']', ' ', text)
    text = re.sub(r'\s+', ' ', text)
    
    return text

def chunk_text_smart(text, tokenizer, max_length=900):
    """Chia văn bản thông minh dựa trên câu và đoạn"""
    sentences = re.split(r'[.!?]+', text)
    chunks = []
    current_chunk = ""
    
    for sentence in sentences:
        sentence = sentence.strip()
        if not sentence:
            continue
            
        # Kiểm tra độ dài token
        test_chunk = current_chunk + " " + sentence if current_chunk else sentence
        token_count = len(tokenizer.encode(test_chunk))
        
        if token_count <= max_length:
            current_chunk = test_chunk
        else:
            if current_chunk:
                chunks.append(current_chunk.strip())
            current_chunk = sentence
    
    if current_chunk:
        chunks.append(current_chunk.strip())
    
    return chunks

def summarize_chunk(chunk, summarizer, max_len=200, min_len=50):
    """Tóm tắt một đoạn văn bản"""
    try:
        # Điều chỉnh độ dài dựa trên kích thước chunk
        chunk_len = len(chunk.split())
        if chunk_len < 100:
            max_len = min(max_len, chunk_len // 2)
            min_len = min(min_len, max_len // 2)
        
        summary = summarizer(
            chunk, 
            max_length=max_len, 
            min_length=max(min_len, 20),
            do_sample=True,
            temperature=0.7,
            num_beams=4,
            early_stopping=True,
            no_repeat_ngram_size=3
        )[0]['summary_text']
        
        return summary
    except Exception as e:
        print(f"Lỗi khi tóm tắt đoạn: {e}")
        return ""

def improve_final_summary(text, summarizer):
    """Cải thiện tóm tắt cuối cùng"""
    try:
        # Tóm tắt lần cuối với các tham số tối ưu
        final_summary = summarizer(
            text,
            max_length=400,
            min_length=100,
            do_sample=True,
            temperature=0.8,
            num_beams=6,
            early_stopping=True,
            no_repeat_ngram_size=3,
            length_penalty=1.2
        )[0]['summary_text']
        
        return final_summary
    except:
        return text

def main():
    if len(sys.argv) != 2:
        print("Usage: python summarize.py <path_to_summary_input.txt>")
        sys.exit(1)

    input_path = sys.argv[1]
    output_path = input_path.replace(".txt", "_summary.txt")

    # Đọc file
    try:
        with open(input_path, "r", encoding="utf-8") as f:
            text = f.read()
    except FileNotFoundError:
        print(f"File not found: {input_path}")
        sys.exit(1)
    except UnicodeDecodeError:
        # Thử với encoding khác
        try:
            with open(input_path, "r", encoding="cp1252") as f:
                text = f.read()
        except:
            print("Không thể đọc file với encoding phù hợp")
            sys.exit(1)

    if len(text.strip()) < 100:
        print("Văn bản quá ngắn để tóm tắt")
        sys.exit(1)

    print("Đang tiền xử lý văn bản...")
    text = preprocess_text(text)
    
    print("Loading summarization model...")
    # Sử dụng mô hình tốt hơn hoặc thử các mô hình khác
    model_options = [
        "facebook/bart-large-cnn",
        "t5-base",
        "google/pegasus-xsum"
    ]
    
    summarizer = None
    tokenizer = None
    
    for model_name in model_options:
        try:
            print(f"Đang thử mô hình: {model_name}")
            summarizer = pipeline(
                "summarization", 
                model=model_name,
                device=0 if torch.cuda.is_available() else -1
            )
            tokenizer = AutoTokenizer.from_pretrained(model_name)
            print(f"Đã tải thành công mô hình: {model_name}")
            break
        except Exception as e:
            print(f"Không thể tải mô hình {model_name}: {e}")
            continue
    
    if not summarizer:
        print("Không thể tải bất kỳ mô hình nào")
        sys.exit(1)

    print("Đang phân tích và chia văn bản...")
    
    # Kiểm tra độ dài và xử lý
    token_count = len(tokenizer.encode(text))
    print(f"Số token: {token_count}")
    
    if token_count <= 900:
        # Văn bản ngắn, tóm tắt trực tiếp
        print("Tóm tắt văn bản ngắn...")
        summary = summarize_chunk(text, summarizer, max_len=300, min_len=80)
    else:
        # Văn bản dài, chia thành chunks
        print("Văn bản dài, đang chia thành các phần...")
        chunks = chunk_text_smart(text, tokenizer, max_length=900)
        print(f"Đã chia thành {len(chunks)} phần")
        
        summaries = []
        for i, chunk in enumerate(chunks):
            print(f"Đang tóm tắt phần {i+1}/{len(chunks)}...")
            chunk_summary = summarize_chunk(chunk, summarizer)
            if chunk_summary:
                summaries.append(chunk_summary)
        
        if not summaries:
            print("Không thể tạo tóm tắt")
            sys.exit(1)
        
        # Kết hợp các tóm tắt
        combined_summary = " ".join(summaries)
        
        # Tóm tắt lần cuối nếu quá dài
        if len(tokenizer.encode(combined_summary)) > 500:
            print("Đang tối ưu tóm tắt cuối cùng...")
            summary = improve_final_summary(combined_summary, summarizer)
        else:
            summary = combined_summary

    # Hậu xử lý tóm tắt
    summary = summary.strip()
    summary = re.sub(r'\s+', ' ', summary)
    
    # Loại bỏ các câu thừa không liên quan
    sentences = re.split(r'[.!?]+', summary)
    filtered_sentences = []
    
    # Từ khóa cần loại bỏ
    remove_keywords = [
        "use the weekly newsquiz", "back to the page", "cnn.com", 
        "test your knowledge", "you came from", "available in e-book",
        "print versions", "other channels"
    ]
    
    for sentence in sentences:
        sentence = sentence.strip()
        if sentence and not any(keyword.lower() in sentence.lower() for keyword in remove_keywords):
            filtered_sentences.append(sentence)
    
    # Ghép lại thành tóm tắt sạch
    clean_summary = '. '.join(filtered_sentences)
    if clean_summary and not clean_summary.endswith('.'):
        clean_summary += '.'
    
    # Lưu kết quả - chỉ nội dung tóm tắt
    try:
        with open(output_path, "w", encoding="utf-8") as f:
            f.write(clean_summary)
        
       
        print(f"Đã lưu tại: {output_path}")
        
    except Exception as e:
        print(f"Lỗi khi lưu file: {e}")

if __name__ == "__main__":
    main()