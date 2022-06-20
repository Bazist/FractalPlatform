using BigDoc.Client;
using BigDoc.Database.Engine;
using System;

namespace FractalPlatform.Examples
{
    public class HelloWorldApplication : BaseApplication
    {
        public HelloWorldApplication(string workingFolder,
                                     IFormFactory formFactory) : base(workingFolder,
                                                                null,
                                                                formFactory)
        {
        }

        public override void OnStart(Context context)
        {
            MessageBox("Hello Fractal World !");
        }
    }
}
