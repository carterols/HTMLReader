using System;
using System.Collections.Generic;
using System.Text;

namespace HTMLReader {
    class HTMLPage {
        private string _url;
        private HTMLElementCollection _htmlElements;
        private int _currentInternalId;
        public HTMLPage() {
            _currentInternalId = 0;
        }

        public void AddElement(HTMLElement elem) {
            _htmlElements.AddElement(elem, _currentInternalId++);
        }
    }
}
