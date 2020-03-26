using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    public class UnknownCardException : ExceptionWithMessageWhyDoesntCSharpHaveItDeafaultComeOne
    {
    public UnknownCardException(string message) : base(message) { }
}
