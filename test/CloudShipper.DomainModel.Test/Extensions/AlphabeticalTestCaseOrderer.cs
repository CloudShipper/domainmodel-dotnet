using Xunit.Abstractions;
using Xunit.Sdk;

namespace CloudShipper.DomainModel.Test.Extensions;

public class AlphabeticalTestCaseOrderer : ITestCaseOrderer
{
    public const string TypeName = "CloudShipper.DomainModel.Test.Extensions.AlphabeticalTestCaseOrderer";
    public const string AssemblyName = "CloudShipper.DomainModel.Test";

    public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases) where TTestCase : ITestCase
        => testCases.OrderBy(t => t.TestMethod.Method.Name);
}
