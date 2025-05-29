using App472.WebUI.Models;
using System.Data.Entity;

namespace App472.WebUI.App_Start.Debug
{
    //public class EFDBInitializer : DropCreateDatabaseAlways<EFDBContext>
    //{
    //    protected override void Seed(EFDBContext context)
    //    {
    //        context.SeedDomainObjects();
    //        base.Seed(context);
    //    }
    //}

    public class IDDBInitializer : DropCreateDatabaseAlways<IDDBContext>
    {
        protected override void Seed(IDDBContext context)
        {
            context.SeedIDContext();
            base.Seed(context);
        }
    }
}