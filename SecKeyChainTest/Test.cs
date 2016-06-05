using System;
using Xunit;
using Security;
using Foundation;

namespace SecKeyChainTest
{
  public class Test
  {
    const string TestAccountName = "this is a test record";
    static readonly NSData TestSecret;
    static readonly NSData TestNewSecret;

    static Test()
    {
      TestSecret = NSData.FromString("this is my secret", NSStringEncoding.UTF8);
      TestNewSecret = NSData.FromString("this is my new secret", NSStringEncoding.UTF8);
    }

    public Test()
    {
    }

    [Fact]
    public void TestUpdate()
    {
      var queryRecord = new SecRecord(SecKind.GenericPassword)
      {
          Account = TestAccountName,
        Accessible = SecAccessible.WhenUnlocked
      };

      SecStatusCode queryStatusCode;
      SecRecord queryResult;
      queryResult = SecKeyChain.QueryAsRecord(queryRecord, out queryStatusCode);
      if (queryStatusCode == SecStatusCode.ItemNotFound)
      {
        var newRecord = new SecRecord(SecKind.GenericPassword)
        {
          Account = TestAccountName,
          Accessible = SecAccessible.WhenUnlocked,
          Generic = TestSecret
        };
        var newRecordStatusCode = SecKeyChain.Add(newRecord);
        Assert.True(newRecordStatusCode == SecStatusCode.Success);
      }

      queryResult = SecKeyChain.QueryAsRecord(queryRecord, out queryStatusCode);
      Assert.True(queryStatusCode == SecStatusCode.Success);
      Assert.True(TestSecret.Equals(queryResult.Generic));

      // try updating
      SecRecord newSecret = new SecRecord(SecKind.GenericPassword)
      {
          Generic = TestNewSecret
      };
      var updateStatusCode = SecKeyChain.Update(queryRecord, newSecret);
      Assert.True(updateStatusCode == SecStatusCode.Success,
        string.Format("Got {0} instead", updateStatusCode));

      queryResult = SecKeyChain.QueryAsRecord(queryRecord, out queryStatusCode);
      Assert.True(queryStatusCode == SecStatusCode.Success);
      Assert.True(TestNewSecret.Equals(queryResult.Generic));
    }
  }
}

