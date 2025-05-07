using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schema.Response
{
    public class ExpenseDocumentResponse
    {

        public long Id { get; set; }

        public int ExpenseId { get; set; }

        public string FileName { get; set; }

        public string FileType { get; set; }

        public string FilePath { get; set; }

        public DateTime UploadedAt { get; set; }
    }
}
