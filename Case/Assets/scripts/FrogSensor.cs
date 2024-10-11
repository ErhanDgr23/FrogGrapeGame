using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gamefrogs
{
    public class FrogSensor : MonoBehaviour
    {
        public bool toplayamaz;
        public List<Grape> uzumler = new List<Grape>();

        [SerializeField] Frog myfrog;
        [SerializeField] int uzunsayi;

        bool listeekleme;
        float speed = 3f;

        private void Start()
        {
            uzunsayi = 0;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "grape")
            {
                if (toplayamaz)
                    return;

                Grape collidegrape = other.gameObject.GetComponent<Grape>();

                if (!listeekleme)
                    uzumler.Add(collidegrape);
                else
                {
                    if (!collidegrape.sayildi)
                        waypointhesapla();
                    else
                        waypointhesapla(true);

                    collidegrape.sayildi = true;
                }

                if (myfrog.mycolor == collidegrape.mycolor)
                    uzumtopla(collidegrape);
                else
                    geridon();
            }

            if (other.gameObject.tag == "rotater" && myfrog.mycolor == other.GetComponent<arrow>().mycolor)
            {
                arrow myarrow = other.gameObject.GetComponent<arrow>();

                if (!myfrog.tonguego)
                {
                    if (!toplayamaz)
                        myarrow.mycell.yokol();

                    transform.position = new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z);
                    myfrog.tonguedirectionvalue = -myarrow.comingdirection;
                    myfrog.tonguedirectionchange(-1);
                }
                else
                {
                    transform.position = new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z);
                    myarrow.comingdirection = -myfrog.tonguedirection;
                    myfrog.tonguedirectionvalue = other.transform.up;
                    myfrog.tonguedirectionchange(1);
                }
            }
            else if (other.gameObject.tag == "out")
                tamamla();

            if (other.gameObject.tag == "frog")
                geridon();
        }

        public void waypointhesapla(bool sayiart = false)
        {
            if (uzumler.Count > uzunsayi)
            {
                if (!sayiart)
                    uzunsayi++;

                uzumler[uzumler.Count - uzunsayi].takibebasla(speed, transform, transform.position - uzumler[uzumler.Count - uzunsayi].transform.position);

                for (int i = uzumler.Count - uzunsayi + 1; i < uzumler.Count; i++)
                {
                    uzumler[i].takibebasla(speed, uzumler[i - 1].transform,
                        uzumler[i - 1].transform.position - uzumler[i].transform.position);
                }
            }
        }

        public void uzumtopla(Grape item)
        {
            myfrog.tonguegrapeslist.Add(item);
            item.scaleanim();
            item.toplandi = true;
        }

        public void tamamla()
        {
            listeekleme = true;
            waypointhesapla();
            myfrog.tonguego = false;
        }

        public void geridon()
        {
            toplayamaz = true;
            gamemanager.manger.checkwinorloose();

            foreach (var item in myfrog.tonguegrapeslist)
            {
                item.toplandi = false;
                item.sayildi = false;
            }

            uzumler.Clear();
            myfrog.tonguegrapeslist.Clear();
            myfrog.tonguego = false;
        }
    }
}
