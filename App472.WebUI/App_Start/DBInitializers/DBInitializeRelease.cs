using App472.WebUI.Models;
using System.Data.Entity;

namespace App472.WebUI.App_Start.Release
{
    //public class EFDBInitializer : CreateDatabaseIfNotExists<EFDBContext>
    //{
    //    protected override void Seed(EFDBContext context)
    //    {
    //        context.SeedDomainObjects();
    //        base.Seed(context);
    //    }
    //}

    public class IDDBInitializer : CreateDatabaseIfNotExists<IDDBContext>
    {
        protected override void Seed(IDDBContext context)
        {
            context.SeedIDContext();
            context.SeedDomainObjects();
            base.Seed(context);
        }
    }
}