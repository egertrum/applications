using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace TestProject.Helpers
{
    public static class HttpClientExtensions
    {
        public static Task<HttpResponseMessage> SendAsync(
            this HttpClient client,
            IHtmlFormElement form,
            IHtmlElement submitButton)
        {
            return client.SendAsync(form, submitButton, new Dictionary<string, string>());
        }

        public static Task<HttpResponseMessage> SendAsync(
            this HttpClient client,
            IHtmlFormElement form,
            IEnumerable<KeyValuePair<string, string>> formValues)
        {
            var submitElement = Assert.Single(form.QuerySelectorAll("[type=submit]"));
            var submitButton = Assert.IsAssignableFrom<IHtmlElement>(submitElement);

            return client.SendAsync(form, submitButton, formValues);
        }

        public static Task<HttpResponseMessage> SendAsync(
            this HttpClient client,
            IHtmlFormElement form,
            IHtmlElement submitButton,
            IEnumerable<KeyValuePair<string, string>> formValues)
        {
            form = SetFormValues(form, formValues);
            
            var submit = form.GetSubmission(submitButton);
            var target = (Uri) submit.Target;
            if (submitButton.HasAttribute("formaction"))
            {
                var formaction = submitButton.GetAttribute("formaction");
                target = new Uri(formaction, UriKind.Relative);
            }

            var submission = new HttpRequestMessage(new HttpMethod(submit.Method.ToString()), target)
            {
                Content = new StreamContent(submit.Body)
            };

            foreach (var (key, value) in submit.Headers)
            {
                submission.Headers.TryAddWithoutValidation(key, value);
                submission.Content.Headers.TryAddWithoutValidation(key, value);
            }

            return client.SendAsync(submission);
        }

        private static IHtmlFormElement SetFormValues(IHtmlFormElement form, IEnumerable<KeyValuePair<string, string>> formValues)
        {
            foreach (var (key, value) in formValues)
            {
                switch (form[key])
                {
                    case IHtmlInputElement:
                    {
                        (form[key] as IHtmlInputElement)!.Value = value;
                        if ((form[key] as IHtmlInputElement)!.Type == "checkbox" && bool.Parse(value))
                        {
                            (form[key] as IHtmlInputElement)!.IsChecked = true;
                        }
                        break;
                    }
                    case IHtmlSelectElement:
                    {
                        (form[key] as IHtmlSelectElement)!.Value = value;
                        break;
                    }
                    case IHtmlTextAreaElement:
                    {
                        (form[key] as IHtmlTextAreaElement)!.Value = value;
                        break;
                    }

                    // todo: fileupload, textarea
                }
            }
            return form;
        }
    }
}