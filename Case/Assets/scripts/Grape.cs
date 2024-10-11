using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;
using static gamefrogs.gamemanager;

namespace gamefrogs
{
    public class Grape : MonoBehaviour
    {
        public gamefrogs.gamemanager.mycolor mycolor;

        [Header("Renkler")]
        [SerializeField] Material yellowMaterial;
        [SerializeField] Material redMaterial;
        [SerializeField] Material purpleMaterial;
        [SerializeField] Material greenMaterial;
        [SerializeField] Material blueMaterial;

        [SerializeField] List<GameObject> objeler;

        [HideInInspector]
        public bool toplandi, sayildi;
        public Cell mycell;
        public List<Vector3> followposes = new List<Vector3>();
        public Transform targetobjectpos;

        Vector3 firstscale, direction;
        public int curretpos;
        float speed;
        bool flwbool, donotfollow;
        public float zmn, cooldown, dist;

        private void Start()
        {
            cooldown = 0.05f;
            firstscale = new Vector3(0.7f, 0.7f, 0.7f);
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

        public void scaleanim()
        {
            if (toplandi)
                return;

            transform.localScale = new Vector3(transform.localScale.x + 0.35f, transform.localScale.y + 0.35f, transform.localScale.z + 0.35f);
            StopCoroutine(beklescaleanim());
            StartCoroutine(beklescaleanim());
        }

        IEnumerator beklescaleanim()
        {
            yield return new WaitForSeconds(0.5f);
            transform.localScale = firstscale;
        }

        public void cellyoket()
        {
            StopCoroutine(beklecellyoket());
            StartCoroutine(beklecellyoket());
        }

        public void takibebasla(float sped, Transform pos,  Vector3 dire)
        {
            transform.SetParent(null);
            cellyoket();
            flwbool = true;
            speed = sped;
            direction = dire;
            targetobjectpos = pos;
            followposes.Add(targetobjectpos.position);
        }

        IEnumerator beklecellyoket()
        {
            yield return new WaitForSeconds(0.1f);
            mycell.yokol();
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "frog")
            {
                if (!toplandi)
                    return;

                donotfollow = true;
                transform.GetComponent<MeshRenderer>().enabled = false;
                transform.position += ((transform.position - new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z)).normalized * -1000f) * Time.deltaTime;
            }
        }

        private void Update()
        {
            if (mycell.istopon)
                gameObject.SetActive(true);
            else
                gameObject.SetActive(false);

            if (transform.localScale != firstscale)
                transform.localScale = Vector3.Lerp(transform.localScale, firstscale, 5f * Time.deltaTime);

            if (donotfollow)
                return;

            if (targetobjectpos == null)
                return;
            if (targetobjectpos != null)
            {
                if (zmn > cooldown)
                {
                    followposes.Add(targetobjectpos.position);
                    zmn = 0f;
                }
                else
                    zmn += Time.deltaTime;
            }

            if (flwbool)
            {
                if(curretpos < followposes.Count)
                {
                    dist = Vector3.Distance(transform.position, followposes[curretpos]);
                    transform.position = Vector3.MoveTowards(transform.position, followposes[curretpos] + (direction.normalized / 5f), speed * Time.deltaTime);
                    if (dist > -0.35f && dist < 0.35f)
                        curretpos++;
                }
            }
        }
    }
}