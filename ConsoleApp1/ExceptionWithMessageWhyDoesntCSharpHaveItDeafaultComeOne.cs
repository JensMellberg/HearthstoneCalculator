using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


  public  class ExceptionWithMessageWhyDoesntCSharpHaveItDeafaultComeOne : Exception
    {
    public string message;
    public ExceptionWithMessageWhyDoesntCSharpHaveItDeafaultComeOne(string message)
    {
        this.message = message;
    }
    }
