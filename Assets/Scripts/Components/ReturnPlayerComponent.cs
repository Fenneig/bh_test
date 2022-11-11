using UnityEngine;
using Random = UnityEngine.Random;

namespace Components
{
    public class ReturnPlayerComponent : MonoBehaviour
    {
        [SerializeField] private Transform[] _spawns;

        public void ReturnPlayer(GameObject player)
        {
            player.transform.position = _spawns[Random.Range(0, _spawns.Length - 1)].position;
        }
    }
}
