﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common;

namespace GameServer.Services
{
    class DBService : Singleton<DBService>
    {
        ExtremeWorldEntities entities;

        public ExtremeWorldEntities Entities
        {
            get { return this.entities; }
        }

        public void Init()
        {
            entities = new ExtremeWorldEntities();
        }

        public void Save(bool async=false)
        {
            //可以回档
            if (async)
            {
                entities.SaveChangesAsync();
            }
            else
            {
                entities.SaveChanges();
            }
        }
    }
}
