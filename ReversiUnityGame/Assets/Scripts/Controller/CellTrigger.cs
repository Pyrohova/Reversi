using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.TestFolder
{
    public class CellTrigger : MonoBehaviour
    {
        [SerializeField] HumanController controller;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnMouseDown()
        {
            controller.PutChip(transform.name);
        }


    }
}
