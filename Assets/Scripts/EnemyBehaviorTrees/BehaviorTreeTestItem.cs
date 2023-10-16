using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using EnemyBehaviorTrees.Agents;
using WUG.BehaviorTreeVisualizer;

namespace EnemyBehaviorTrees.Agents
{
    public class BehaviorTreeTestItem : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (BehaviorTreeTestGameManager.Instance.NPC.MyActivity == NavigationActivity.PICKUP_ITEM)
            {
                BehaviorTreeTestGameManager.Instance.PickupItem(this.gameObject);
            }
        }
    }
}