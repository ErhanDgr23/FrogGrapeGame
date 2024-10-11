using gamefrogs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static gamefrogs.gamemanager;

namespace gamefrogs
{
    public class arrow : MonoBehaviour
    {
        public gamefrogs.gamemanager.mycolor mycolor;
        public Vector3 comingdirection;

        [Header("Renkler")]
        [SerializeField] Material yellowMaterial;
        [SerializeField] Material redMaterial;
        [SerializeField] Material purpleMaterial;
        [SerializeField] Material greenMaterial;
        [SerializeField] Material blueMaterial;

        [HideInInspector]
        public Cell mycell;

        private void Start()
        {
            mycell = transform.parent.GetComponent<Cell>();
            transform.GetComponent<MeshRenderer>().material = getcolorfromcell(mycell.mycolor);
            mycolor = mycell.mycolor;
        }

        Material getcolorfromcell(mycolor color)
        {
            switch (color)
            {
                case mycolor.yellow:
                    return yellowMaterial;

                case mycolor.red:
                    return redMaterial;

                case mycolor.purple:
                    return purpleMaterial;

                case mycolor.green:
                    return greenMaterial;

                case mycolor.blue:
                    return blueMaterial;

                default:
                    return null;
            }
        }
    }
}