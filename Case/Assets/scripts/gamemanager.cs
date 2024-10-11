using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace gamefrogs
{
    public class gamemanager : MonoBehaviour
    {
        public static gamemanager manger; 

        public enum mycolor
        {
            yellow,
            red,
            purple,
            green,
            blue
        };

        [SerializeField] LayerMask raylayer;
        [SerializeField] GameObject winpan, failpan;
        [SerializeField] TextMeshProUGUI moveindicator;
        [SerializeField] int moveindex;

        bool gamefinish;
        Frog[] kurbalar;
        Ray ray;
        RaycastHit hit;
        Camera kam;

        private void Awake()
        {
            manger = this;
        }

        private void Start()
        {
            kurbalar = FindObjectsOfType<Frog>();
            kam = Camera.main;
        }

        private void Update()
        {
            ray = kam.ScreenPointToRay(Input.mousePosition);
            moveindicator.text = moveindex.ToString();

            if (Input.GetButtonDown("Fire2"))
            {
                if (Physics.Raycast(ray, out hit, 50f, raylayer))
                {
                    if (hit.collider.gameObject != null)
                    {
                        if(moveindex > 0)
                        {
                            //print(hit.collider.transform.name);
                            if (hit.collider.gameObject.tag == "frog")
                            {
                                hit.transform.GetComponent<Frog>().tiklandi();
                                moveindex--;
                            }
                        }
                    }
                }
            }
        }

        public void checkwinorloose()
        {
            StopCoroutine(beklecheck());
            StartCoroutine(beklecheck());
        }

        IEnumerator beklecheck()
        {
            yield return new WaitForSeconds(0.5f);

            if (moveindex > 0)
                yield break;

            bool allNull = true;

            for (int i = 0; i < kurbalar.Length; i++)
            {
                if (kurbalar[i] != null)
                {
                    allNull = false;

                    if (kurbalar[i].toplaniyor)
                        yield break;

                    if (!allNull && moveindex <= 0f)
                    {
                        failed();
                        gamefinish = true;
                        yield break;
                    }
                }
            }

            if (!gamefinish)
            {
                win();
            }
        }

        public void levelnextorreset(int levelindex)
        {
            SceneManager.LoadScene(levelindex);
        }

        public void failed()
        {
            failpan.gameObject.SetActive(true);
        }

        public void win()
        {
            winpan.gameObject.SetActive(true);
            gamefinish = true;
        }
    }
}