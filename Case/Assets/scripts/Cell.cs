using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;
using static gamefrogs.gamemanager;

namespace gamefrogs
{
    public class Cell : MonoBehaviour
    {
        public gamefrogs.gamemanager.mycolor mycolor;
        public bool istopon;
        public layercontroller mylayercont;

        [SerializeField] GameObject myobject;

        [Header("Prefabs")]
        [SerializeField] GameObject frogpre;
        [SerializeField] GameObject grapepre;
        [SerializeField] GameObject rotaterpre;

        [Header("Renkler")]
        [SerializeField] Material yellowMaterial;
        [SerializeField] Material redMaterial;
        [SerializeField] Material purpleMaterial;
        [SerializeField] Material greenMaterial;
        [SerializeField] Material blueMaterial;

        BoxCollider collidercomp;
        public bool yokolb, scaleanimb;
        Vector3 firscalemyobject;

        #region editorlayout
        public void spawnchild(string name)
        {
            if(name == "frog")
            {
                GameObject GO = Instantiate(frogpre, transform);
                GO.transform.position = transform.position + new Vector3(0f, 0.125f, 0f);
            }

            if (name == "grape")
            {
                GameObject GO = Instantiate(grapepre, transform);
                GO.transform.position = transform.position + new Vector3(0f, 0.125f, 0f);
                GO.transform.eulerAngles = new Vector3(0f, 180f, 0f);
            }

            if (name == "rotater")
            {
                GameObject GO = Instantiate(rotaterpre, transform);
                GO.transform.position = transform.position + new Vector3(0f, 0.04f, 0f);
            }

            if (name == "child")
            {
                if (transform.childCount > 0)
                    DestroyImmediate(transform.GetChild(0).gameObject);
            }
        }
        #endregion

        private void Start()
        {
            UpdateMaterial();

            if (transform.childCount != 0)
                myobject = transform.GetChild(0).transform.gameObject;
        }

        private void OnValidate()
        {
            UpdateMaterial();
        }

        private void UpdateMaterial()
        {
            if (transform.TryGetComponent<MeshRenderer>(out MeshRenderer renderer))
            {
                renderer.material = GetMaterial(mycolor);
            }
        }

        private void Update()
        {
            collidercomp = GetComponent<BoxCollider>();

            if (istopon)
            {
                collidercomp.enabled = true;

                if (myobject != null)
                    myobject.SetActive(true);
            }
            else
            {
                collidercomp.enabled = false;

                if (myobject != null)
                    myobject.SetActive(false);
            }

            if(yokolb)
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 3f * Time.deltaTime);

            if (scaleanimb)
                myobject.transform.localScale = Vector3.Lerp(myobject.transform.localScale, firscalemyobject, 10f * Time.deltaTime);
        }

        public void myobjectanimation()
        {
            if (myobject == null)
                return;

            firscalemyobject = myobject.transform.localScale;
            myobject.transform.localScale = Vector3.zero;
            scaleanimb = true;
            StopCoroutine(beklescaleanim());
            StartCoroutine(beklescaleanim());
        }

        IEnumerator beklescaleanim()
        {
            yield return new WaitForSeconds(0.7f);
            scaleanimb = false;
            myobject.transform.localScale = firscalemyobject;
        }

        public void yokol(float time = 1f)
        {
            if (yokolb)
                return;

            yokolb = true;
            StartCoroutine(bekleyokol(time));
        }

        IEnumerator bekleyokol(float time)
        {
            yield return new WaitForSeconds(time);
            mylayercont.listedencikar(this);
            mylayercont.cellactiveter();
            this.gameObject.SetActive(false);
        }

        Material GetMaterial(mycolor color)
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