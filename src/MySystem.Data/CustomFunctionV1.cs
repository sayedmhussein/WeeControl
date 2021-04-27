using System;
namespace MySystem.Data
{
    public class CustomFunctionV1
    {
        private readonly DataContext context;

        public CustomFunctionV1(DataContext context)
        {
            this.context = context;
        }
    }
}
