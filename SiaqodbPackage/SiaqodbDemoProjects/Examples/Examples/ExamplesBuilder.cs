using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SiaqodbExamples
{
    public class ExamplesBuilder
    {
        public static IList<IExample> GetExamples(WriteLine logger)
        {
            List<IExample> examples = new List<IExample>();
            examples.Add(new SimpleExample() { Log=logger});
           examples.Add(new POCOExample() { Log = logger });
            examples.Add(new UsingAttributesExample() { Log = logger });
            examples.Add(new JoinExample() { Log = logger });
            examples.Add(new InheritanceExample() { Log = logger });
           examples.Add(new IndexExample() { Log = logger });
            examples.Add(new EncryptionExample() { Log = logger });
            examples.Add(new AllTypesOfFieldsSupportedExample() { Log = logger });
            examples.Add(new UpdateObjectByExample() { Log = logger });
            examples.Add(new DynamicLINQExample() { Log = logger });
            examples.Add(new CustomDatabaseFileNameExample() { Log = logger });
            examples.Add(new LINQ_OptimizedExamples() { Log = logger });
            examples.Add(new LINQ_CrashExamples() { Log = logger });
            examples.Add(new ListsArraysExample() { Log = logger });
            return examples;
        }
    }
}
