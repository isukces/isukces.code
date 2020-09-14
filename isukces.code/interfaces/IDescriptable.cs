using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSukces.Code.Interfaces
{
    public interface IDescriptable
    {
        string Description { get; set; }
    }

    public interface ICommentable
    {
        void AddComment(string x);
        string GetComments();
    }
}
