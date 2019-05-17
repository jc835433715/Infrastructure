using Infrastructure.Common.Interface;
using System;

namespace Infrastructure.ComPort.Interface
{
    /// <summary>
    /// ͨѶ�˿ڽӿ�
    /// </summary>
    public interface IComPort : IDisposable, INotifyConnectionStateChanged
    {

        /// <summary>
        /// �Ƿ�������
        /// </summary>
        bool IsConnected
        {
            get;
        }

        /// <summary>
        /// �˿���
        /// </summary>
        string Name { get; }

        /// <summary>
        ///�ɶ�ȡ���ֽ���
        /// </summary>
        int BytesToRead
        {
            get;
        }

        /// <summary>
        /// ���Զ�����
        /// </summary>
        ///<param name="isAsync">�Ƿ��첽</param>
        void Open(bool isAsync = true);

        /// <summary>
        /// д��
        /// </summary>
        /// <param name="buffer">�ֽ�����</param>
        /// <param name="offset">�ֽ�ƫ����</param>
        /// <param name="count">�ֽ���</param>
        void Write(byte[] buffer, int offset, int count);

        /// <summary>
        /// ��ȡ
        /// </summary>
        /// <param name="buffer">�ֽ�����</param>
        /// <param name="offset">�ֽ�ƫ����</param>
        /// <param name="count">����ȡ���ֽ���</param>
        /// <returns>����ʵ�ʶ�ȡ�ֽ���</returns>
        int Read(byte[] buffer, int offset, int count);

        /// <summary>
        /// �ر�
        /// </summary>
        void Close();
    }
}
