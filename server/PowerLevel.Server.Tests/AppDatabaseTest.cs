namespace PowerLevel.Server.Tests;

using Infrastructure;
using Xdxd.DotNet.Testing;

public abstract class AppDatabaseTest : DatabaseTest<IDbService, DbPocos>
{
    protected AppDatabaseTest() : base(
        "../before-app-tests.sql",
        TestHelper.TestConfig.TestMasterConnectionString,
        x => new DbService(x)
    ) { }
}
