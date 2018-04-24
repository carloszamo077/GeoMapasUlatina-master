namespace GeoMapasUlatina.Helpers
{

    using System;
    using Realms;

    public class UtilDB
    {

        public static Realm GetInstanceRealm()
        {

            var config = new RealmConfiguration()
            {
                SchemaVersion = 1,
                MigrationCallback = (migration, oldSchemaVersion) =>
                {

                }
            };

            var _realm = Realm.GetInstance(config);
            return _realm;

        }

    }
}
