using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using UnityEngine;

namespace Managers
{
    class MinimapManager:Singleton<MinimapManager>
    {
        public Sprite LoadCurrenMinimap()
        {
            Sprite s = Resloader.Load<Sprite>("UI/Minimap/" + User.Instance.CurrentMapData.MinMap);
            return s;
        }

    }
}
