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

        public Transform PlayerTransform
        {
            get
            {
                if (User.Instance.CurrentCharacterObject == null)
                {
                    return null;
                }

                return User.Instance.CurrentCharacterObject.transform;
            }
        }

        public Sprite LoadCurrenMinimap()
        {
            Sprite s = Resloader.Load<Sprite>("UI/Minimap/" + User.Instance.CurrentMapData.MinMap);
            return s;
        }

    }
}
