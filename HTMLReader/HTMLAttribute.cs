namespace HTMLReader {
    class HTMLAttribute {
        public string Name { get; set; }
        public string Value { get; set; }
        public HTMLAttribute(string name, string value) {
            Name = name;
            Value = value;
        }
    }
}