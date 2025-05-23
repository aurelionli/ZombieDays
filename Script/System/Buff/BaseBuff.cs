using System;
using System.Collections.Generic;
using UnityEngine;


namespace FPS
{
    public abstract class BaseBuff:IController
    {
        protected IController IC;

        public float duration; //����ʱ�䣬-1��ʾ����
        public float elapsedTime; //buff �ѳ���ʱ��
        public float count;     //��������
        public float tickInterval;//�������
        public bool isKeepWorking;//�ǲ��ǳ�����buff


        public BaseBuff()
        {
            IC = this;
       
            elapsedTime = 0f;
           
        }



        public abstract void OnApply();//buff ��Ч��ʱ��

        public abstract void OnRemove();//buff �Ƴ���ʱ��

        public abstract void OnTick();//�����������߼�
       
    }

}