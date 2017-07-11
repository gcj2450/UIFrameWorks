using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sqo;
using Sqo.Attributes;
namespace SiaqodbExamples
{
    class UpdateObjectByExample : BaseExample, IExample
    {
       

        public void Run()
        {
            //it is possible that you have and object at runtime that came from server side or similar but is not loaded from Siaqodb database
            //and you need to update or delete it only, without load, in this case UpdateObjectBy/DeleteObjectBy method can be used like:
            
            //prepare some data
            Guid IdKept = Guid.Empty;
            Siaqodb siaqodb = SiaqodbFactoryExample.GetInstance();
            for (int i = 0; i < 10; i++)
            {
                RecordWithManualID rec = new RecordWithManualID { ID = Guid.NewGuid(), Name = "Name" + i.ToString() };

                siaqodb.StoreObject(rec);
                if (i == 5)
                {
                    IdKept = rec.ID;
                }
            }

            
            RecordWithManualID recNew = new RecordWithManualID();
            recNew.Name = "Updated Name";
            recNew.ID = IdKept;

            //update object by ID(similar in relational databases with "UPDATE ....WHERE ID=yourID"
            siaqodb.UpdateObjectBy("_id", recNew);
            //or use overloaded method:
            //siaqodb.UpdateObjectBy(recNew, new string[] { "_id" });

            var q = from RecordWithManualID r in siaqodb
                    where r.ID == IdKept
                    select r;
            foreach (RecordWithManualID recc in q)
            {
                Log("Record with ID:" + recc.ID.ToString() + " was updated with UpdateObjectBy method and now it has Name=" + recc.Name);

            }

            //DeleteObjectBy works similar(relational DB equivalent: DELETE FROM ... WHERE Id=yourId):

            siaqodb.DeleteObjectBy(recNew, "_id");
            Log("Record with ID:" + recNew.ID.ToString() + " was deleted with DeleteObjectBy from database");

        }

        
    }
    public class RecordWithManualID
    {
        public int OID { get; set; }
        
        private Guid _id;
        [UseVariable("_id")]
		public Guid ID { get { return _id; } set { _id = value; } }

        public string Name { get; set; }
    }
}
