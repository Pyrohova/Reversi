﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.TestFolder
{
    public class CellTrigger : MonoBehaviour
    {
        [SerializeField] GameObject chip;
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
            Instantiate(chip, transform.position, chip.transform.rotation);
        }
    }
}
