using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using static gamefrogs.gamemanager;

namespace gamefrogs
{
    public class Frog : MonoBehaviour
    {
        public gamefrogs.gamemanager.mycolor mycolor;
        public FrogSensor mysensor;

        [SerializeField] LineRenderer frogtongue;

        [Header("Renkler")]
        [SerializeField] Material yellowMaterial;
        [SerializeField] Material redMaterial;
        [SerializeField] Material purpleMaterial;
        [SerializeField] Material greenMaterial;
        [SerializeField] Material blueMaterial;

        [HideInInspector]
        public bool tonguego, directionchange, toplaniyor;
        public List<Grape> tonguegrapeslist;
        public Vector3 tonguedirectionvalue, tonguedirection;

        bool yokol;
        int tonguespot = 2;
        Cell mycell;
        Vector3 sensorfirstpos;
        Animator anim;

        private void Start()
        {
            anim = GetComponent<Animator>();
            sensorfirstpos = frogtongue.transform.position;
            mycell = transform.parent.GetComponent<Cell>();
            transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material = getcolorfromcell(mycell.mycolor);
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

        private void Update()
        {
            if (mycell.istopon)
                gameObject.SetActive(true);
            else
                gameObject.SetActive(false);

            float dist = Vector3.Distance(mysensor.transform.position, sensorfirstpos);

            if (dist > -0.1f && dist <= 0.1f)
            {
                anim.SetBool("mouth", false);

                if (tonguegrapeslist.Count > 1)
                {
                    if (!yokol)
                    {
                        StartCoroutine(beklekurbayokol());
                        yokol = true;
                    }
                }

                tonguedirection = -mysensor.transform.forward;
                mysensor.toplayamaz = false;
            }
            else
                anim.SetBool("mouth", true);

            if (tonguego)
            {
                toplaniyor = true;
                mysensor.transform.Translate(tonguedirection * 3.5f * Time.deltaTime, Space.World);
                frogtongue.positionCount = tonguespot;
                frogtongue.SetPosition(tonguespot - 1, new Vector3(mysensor.transform.localPosition.x, 0.15f, mysensor.transform.localPosition.z));
            }
            else if (dist < -0.05f || dist > 0.05f)
            {
                frogtongue.SetPosition(tonguespot - 1, new Vector3(mysensor.transform.localPosition.x, 0.15f, mysensor.transform.localPosition.z));
                mysensor.transform.Translate(-tonguedirection * 3.5f * Time.deltaTime, Space.World);
            }
        }

        public void tonguedirectionchange(int i)
        {
            tonguedirection = tonguedirectionvalue;
            frogtongue.SetPosition(tonguespot - 1, new Vector3(mysensor.transform.localPosition.x, 0.15f, mysensor.transform.localPosition.z));
            tonguespot += i;
            frogtongue.positionCount = tonguespot;
            directionchange = false;
        }

        IEnumerator beklekurbayokol()
        {
            yield return new WaitForSeconds(1.15f);
            mycell.yokol(0.35f);
            toplaniyor = false;
            gamefrogs.gamemanager.manger.checkwinorloose();
            Destroy(this.gameObject);

            foreach (var item in tonguegrapeslist.ToArray())
            {
                Destroy(item.gameObject);
                tonguegrapeslist.Remove(item);
            }

            if (tonguegrapeslist.Count > 0)
                tonguegrapeslist.Clear();
        }

        public void tiklandi()
        {
            tonguego = true;
        }
    }
}