using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Http;
using System.IO;

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

        public async void DownloadAsync(string url) {
            using (HttpClient client = new HttpClient()) {
                using (HttpResponseMessage res = client.GetAsync(url).Result) {
                    using (HttpContent content = res.Content) {
                        Stream stream = await content.ReadAsStreamAsync();
                        using (StreamReader reader = new StreamReader(stream)) {
                            string line;
                            while ((line = reader.ReadLine()) != null) {
                                _GenerateTags(line);
                            }
                        }
                    }
                }
            }
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
                    } 
                    else if (line[i] == '<' && !insideDoubleQuotes && !   insideSingleQuotes) {
                        insideTags = true;
                        insideTagName = true;
                        if (elementsQueued.Count != 0) {

                        }
                        HTMLElement e = new HTMLElement();
                        elementsQueued.Push(e);
                        continue;
                    } else if (line[i] == '"') {
                        if (insideDoubleQuotes) {
                            insideDoubleQuotes = false;
                            if (attributes.Count != 0) {
                                attributes[attributes.Count - 1].Value = attributeValue.ToString();
                            }
                        }
                        else if ()
                    }
                    if (insideTagName) {
                        if (line[i] == ' ') {
                            insideTagName = false;
                            insideAttribute = true;
                            continue;
                        } else {
                            tagName.Append(line[i]);
                        }
                    } else if (insideAttribute)

                }
                

            }
        }

     


    }
}
