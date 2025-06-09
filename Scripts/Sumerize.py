import sys
from transformers import AutoTokenizer, AutoModelForSeq2SeqLM
import os

def summarize_text(text):
    model_name = "VietAI/vit5-base-vietnews-summarization"
    tokenizer = AutoTokenizer.from_pretrained(model_name)
    model = AutoModelForSeq2SeqLM.from_pretrained(model_name)

    input_ids = tokenizer("summarize: " + text, return_tensors="pt", max_length=512, truncation=True).input_ids
    summary_ids = model.generate(
        input_ids,
        max_length=200,
        min_length=30,
        length_penalty=2.0,
        num_beams=4,
        early_stopping=True
    )

    summary = tokenizer.decode(summary_ids[0], skip_special_tokens=True)
    return summary

def main():
    if len(sys.argv) < 2:
        print("❌ Vui lòng cung cấp đường dẫn tới file văn bản.")
        print("Ví dụ: python Summarize.py <duong_dan_file_input>")
        sys.exit(1)

    input_path = sys.argv[1]

    if not os.path.exists(input_path):
        print(f"❌ File không tồn tại: {input_path}")
        sys.exit(1)

    with open(input_path, "r", encoding="utf-8") as f:
        text = f.read()

    print("🔄 Đang tóm tắt văn bản...")
    summary = summarize_text(text)

    # Tạo đường dẫn file output
    base, ext = os.path.splitext(input_path)
    output_path = base + "_summary.txt"

    with open(output_path, "w", encoding="utf-8") as f:
        f.write(summary)

    print(f"✅ Đã tóm tắt. Kết quả lưu tại: {output_path}")

if __name__ == "__main__":
    main()
