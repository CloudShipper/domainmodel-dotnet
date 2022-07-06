using Xunit.Abstractions;
using Xunit.Sdk;

namespace CloudShipper.DomainModel.EntityFrameworkCore.Test.Extensions;

public class AlphabeticalTestCaseOrderer : ITestCaseOrderer
{
    public const string TypeName = "CloudShipper.DomainModel.EntityFrameworkCore.Test.Extensions.AlphabeticalTestCaseOrderer";
    public const string AssemblyName = "CloudShipper.DomainModel.EntityFrameworkCore.Test";

    public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases) where TTestCase : ITestCase
        => testCases.OrderBy(t => t.TestMethod.Method.Name);
}
