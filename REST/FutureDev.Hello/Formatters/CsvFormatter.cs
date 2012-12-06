using System.Net;
using System.Security.Policy;
using System.Threading.Tasks;
using FutureDev.Hello.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web;

namespace FutureDev.Hello.Formatters
{
    public class CsvFormatter : MediaTypeFormatter
    {
        public CsvFormatter() 
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/csv"));
        }

        public override bool CanWriteType(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            foreach (Type interfaceType in type.GetInterfaces())
            {
                if (interfaceType == typeof(IEnumerable))
                    return true;
            }

            return false;
        }

        public override bool CanReadType(Type type)
        {
            return false;
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, System.Net.Http.HttpContent content, TransportContext transportContext)
        {
            return Task.Factory.StartNew(() =>
            {
                if (type == typeof(IEnumerable<Todo>))
                {
                    WriteToStream(type, value, writeStream, content.Headers);
                }
            });
        }

        public void WriteToStream(Type type, object value, Stream stream, HttpContentHeaders contentHeaders)
        {
            using (var writer = new StreamWriter(stream))
            {

                var todoes = value as IEnumerable<Todo>;
                if (todoes != null)
                {
                    foreach (var todo in todoes)
                    {
                        WriteItem(todo, writer);
                    }
                }
                else
                {
                    throw new InvalidOperationException("Cannot serialize type");
                }
            }

            stream.Close();
        }

        private void WriteItem(Todo todo, StreamWriter writer)
        {
            writer.WriteLine("{0},{1},{2}", Escape(todo.TodoId),Escape(todo.Task), Escape(todo.IsDone));
        }
        
        private string Escape(object o)
        {
             var _specialChars = new char[] { ',', '\n', '\r', '"' };

            if (o == null)
            {
                return "";
            }

            string field = o.ToString();
            if (field.IndexOfAny(_specialChars) != -1)
            {
                return String.Format("\"{0}\"", field.Replace("\"", "\"\""));
            }

            else return field;
        }

    }
}