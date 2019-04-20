namespace HTMLReader {
    internal class HTMLElementCollection {
        private HTMLElement _first;
        private HTMLElement _last;
        private HTMLElement[] _inBetweenPtrs;
        private int _currentInternalId;
        public HTMLElementCollection() {
            _first = null;
            _last = null;
            _inBetweenPtrs = null;
            _currentInternalId = 0;
        }

        public void AddElement(HTMLElement element, int internalId) {
            element.setInternalId(internalId);
            if (_first == null) {
                _first = element;
            }
            if (_last != null) {
                // if the parents are the same
                if (_last.Parent.getInternalId() == element.Parent.getInternalId()) {
                    _last.NextSibling = element;
                } else if (_last.getInternalId() == element.Parent.getInternalId()) {
                    _last.Children.AddElement(element, internalId);
                }
                _last.NextSibling = element;
            }
            _last = element;
        }
    }
}