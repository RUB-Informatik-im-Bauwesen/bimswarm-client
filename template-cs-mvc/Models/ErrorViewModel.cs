using System;

namespace Bimswarm.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public string MessageTitle { get; set; }

        public string MessageText { get; set; }
        public string MessageTextDetails { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public ErrorViewModel(string title, string text, string details) {
            this.MessageTitle = title;
            this.MessageText = text;
            this.MessageTextDetails = details;
        }
    }
}