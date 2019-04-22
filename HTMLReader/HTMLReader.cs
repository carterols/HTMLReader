using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Threading.Tasks;

namespace HTMLReader {
    class HTMLReader {
        public bool DefaultCredentials = false;
        public HTMLPage HTMLPage;
        public string URL { get; set; }
        public string Method { get; set; }
        public HTMLReader() {}

        private bool insideTags = false;
        private bool insideDoubleQuotes = false;
        private bool insideSingleQuotes = false;
        private bool insideTagName = false;
        private bool insideAttribute = false;
        private bool insideId = false;
        private bool insideClassName = false;   

        private StringBuilder tagName;
        private StringBuilder className;
        private StringBuilder id;
        private StringBuilder attributeValue;
        private StringBuilder attributeName;
        private List<HTMLAttribute> attributes;

        private List<HTMLElement> elementsCached;
        private Stack<HTMLElement> elementsQueued;

        public async Task<string> DownloadAsync(string url) {
            string html;
            using (HttpClient client = new HttpClient()) {
                using (HttpResponseMessage res = await client.GetAsync(url)) {
                    using (HttpContent content = res.Content) {
                        html = await content.ReadAsStringAsync();
                    }
                }
            }
            return html;
        }

        private void _GenerateTags(string line) {
            for (int i = 0; i < line.Length; i++) {
                if (elementsQueued.Count != 0) {
                    if (line[i] == '/' && !insideDoubleQuotes && !insideSingleQuotes) {
                        HTMLElement tmp = elementsQueued.Pop();
                        tmp.ClassName = className.ToString();
                        tmp.Id = id.ToString();
                        tmp.Tag = tagName.ToString();
                        tmp.Parent = elementsQueued.Peek();

                        foreach (HTMLAttribute attr in attributes) {
                            tmp.Attributes.Add(attr);
                        }

                        elementsCached.Add(tmp);
                        insideTags = false;
                        i += tagName.Length + 1;
                    } else if (line[i] == '<' && line[i+1] != '/') {
                        insideTags = true;
                        insideTagName = true;
                        HTMLElement e = new HTMLElement();
                        if (elementsQueued.Count != 0) {
                            e.Parent = elementsQueued.Peek();
                        }
                        elementsQueued.Push(e);
                        i += GetStringUntilDelimeter(ref tagName, line, i + 1, ' ');
                        i += GetAttributes(line, i + 1, ref e);
                        e.Attributes = attributes;
                        attributes.Clear();
                    }

                }
            }
        }

        private int GetStringUntilDelimeter(ref StringBuilder sb, string rawHtml, int currentIdx, char delimeter) {
            int i;
            for (i = currentIdx; i < rawHtml.Length; i++) {
                if (rawHtml[i] == delimeter) { 
                    return i;
                } else {
                    sb.Append(rawHtml[i]);
                }
            }
            return i;
        }

        private int GetAttributes(string rawHtml, int currentIdx, ref HTMLElement e) {
            int i = currentIdx;
            while (i < rawHtml.Length && rawHtml[i] != '>') {
                StringBuilder attrName = new StringBuilder();
                StringBuilder attrVal = new StringBuilder();

                i += GetAttributeName(ref attrName, rawHtml, i);
                for (int j = i + 1; j < rawHtml.Length; j++) {
                    if (rawHtml[j] == '"') {
                        insideDoubleQuotes = true;
                        i += GetStringUntilDelimeter(ref attrVal, rawHtml, j + 1, '"');
                        break;
                    } else if (rawHtml[j] == '\'') {
                        insideSingleQuotes = true;
                        i += GetStringUntilDelimeter(ref attrVal, rawHtml, j + 1, '\'');
                        break;
                    }
                }

                HTMLAttribute attr = new HTMLAttribute(attrName.ToString(), attrVal.ToString());
                if (attrName.ToString().TrimStart() == "class") {
                    e.ClassName = attrVal.ToString();
                } else if (attrName.ToString().TrimStart() == "id") {
                    e.Id = attrVal.ToString();
                }

                attributes.Add(attr);
                i++; 
            }
            return i - 1;
        }
        private int GetAttributeName(ref StringBuilder sb, string rawHtml, int currentIdx) {
            int i;
            for (i = currentIdx; i < rawHtml.Length; i++) {
                if (rawHtml[i] == '=') {
                    return i;
                } else {
                    sb.Append(rawHtml[i]);
                }
            }
           
            return i;
        }
    }
}
