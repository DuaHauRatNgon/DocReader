﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Domain {
        public class Tag {
            public Guid Id { get; set; }
            public string Name { get; set; }

        public ICollection<DocumentTag> DocumentTags { get; set; } = new List<DocumentTag>();
    }
}
