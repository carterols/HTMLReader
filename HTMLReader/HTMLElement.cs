using System;
using System.Collections.Generic;
using System.Text;

namespace HTMLReader {
    class HTMLElement {
        private int _internalId;
        public string InnerHTML { get; set; }
        public string InnerText { get; set; }
        public HTMLElement NextSibling { get; set; }
        public HTMLElement PreviousSibling { get; set; }
        public HTMLElement Parent { get; set; }
        public HTMLElement FirstChild { get; set; }
        public HTMLElement LastChild { get; set; }
        public HTMLElementCollection Children { get; }
        public string Id { get; set; }
        public string ClassName { get; set; }
        public string Tag { get; set; }
        public List<HTMLAttribute> Attributes { get; set; }
        public HTMLElement() : this(String.Empty, String.Empty, 
            String.Empty, String.Empty, 
            String.Empty, null, null, null) {}
        
        public HTMLElement(string inHTML, string inText, string id, 
            string className, string tag, HTMLElement pSibling, HTMLElement parent, HTMLAttribute[] attributes) {
            InnerHTML = inHTML;
            InnerText = inText;
            Id = id;
            ClassName = className;
            Tag = tag;

            // Set links
            PreviousSibling = pSibling;
            pSibling.NextSibling = this;
            NextSibling = null;
            Parent = parent;
            FirstChild = null;
            LastChild = null;
            Children = null;
        }

        internal int getInternalId() {
            return _internalId;
        }
        internal void setInternalId(int id) {
            _internalId = id;
        } 
    }
}
