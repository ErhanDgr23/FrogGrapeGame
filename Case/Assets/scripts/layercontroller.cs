using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gamefrogs
{
    public class layercontroller : MonoBehaviour
    {
        [SerializeField] List<Cell> MyCells = new List<Cell>();

        private void Start()
        {
            MyCells = GetAllChildObjects();
            cellactiveter();
        }

        List<Cell> GetAllChildObjects()
        {
            int childCount = transform.childCount;

            for (int i = 0; i < childCount; i++)
            {
                MyCells.Add(transform.GetChild(i).GetComponent<Cell>());
                MyCells[i].mylayercont = this;
            }

            return MyCells;
        }

        public void cellactiveter()
        {
            if(MyCells.Count > 0)
            {
                MyCells[MyCells.Count - 1].istopon = true;
                MyCells[MyCells.Count - 1].myobjectanimation();
            }
        }

        public void listedencikar(Cell item)
        {
            MyCells.Remove(item);
        }
    }
}