// <auto-generated />
namespace ADYC.API.Auth.Migrations
{
    using System.CodeDom.Compiler;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Infrastructure;
    using System.Resources;
    
    [GeneratedCode("EntityFramework.Migrations", "6.2.0-61023")]
    public sealed partial class AddedFirstNameAndLastNameToUserTable : IMigrationMetadata
    {
        private readonly ResourceManager Resources = new ResourceManager(typeof(AddedFirstNameAndLastNameToUserTable));
        
        string IMigrationMetadata.Id
        {
            get { return "201811210316128_AddedFirstNameAndLastNameToUserTable"; }
        }
        
        string IMigrationMetadata.Source
        {
            get { return null; }
        }
        
        string IMigrationMetadata.Target
        {
            get { return Resources.GetString("Target"); }
        }
    }
}
