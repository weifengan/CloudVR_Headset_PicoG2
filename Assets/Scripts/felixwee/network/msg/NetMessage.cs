/*************************************
			
		NetMessage 
		
   @desction:消息对象类
   @author:felixwee
   @email:felixwee@163.com
   @website:www.felixwee.com
  
***************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class NetMessage
{

    private NetMessageId _msgId;
    private byte[] _body;
    private byte[] _buffers;
    public NetMessage(NetMessageId id)
    {
        this._msgId = id;
        //创建数组
        _buffers = new byte[8];
        //写入Id
        byte[] idbytes = BitConverter.GetBytes((int)this._msgId);
        Buffer.BlockCopy(idbytes, 0, _buffers, 0, idbytes.Length);

        //构建空消息体，长度为0
        byte[] lenbytes = BitConverter.GetBytes(0);
        Buffer.BlockCopy(lenbytes, 0, _buffers, 4, lenbytes.Length);

        this._body = new byte[0];
    }

    public void WriteJson(string json)
    {
        this._body = Encoding.UTF8.GetBytes(json);

        //转化msgId
        byte[] idbytes = BitConverter.GetBytes((int)this._msgId);

        byte[] bodybytes = Encoding.UTF8.GetBytes(json);

        //构建消息体长度
        byte[] lenbytes = BitConverter.GetBytes(bodybytes.Length);

        //创建消息整体bytes
        _buffers = new byte[4 + 4 + bodybytes.Length];

        Buffer.BlockCopy(idbytes, 0, _buffers, 0, idbytes.Length);
        Buffer.BlockCopy(lenbytes, 0, _buffers, 4, lenbytes.Length);
        Buffer.BlockCopy(bodybytes, 0, _buffers, 8, bodybytes.Length);
    }

    public void WriteBytes(byte[] bytes)
    {
        this._body = bytes;

        //转化msgId
        byte[] idbytes = BitConverter.GetBytes((int)this._msgId);

        //构建消息体长度
        byte[] lenbytes = BitConverter.GetBytes(bytes.Length);

        //创建消息整体bytes
        _buffers = new byte[4 + 4 + bytes.Length];

        Buffer.BlockCopy(idbytes, 0, _buffers, 0, idbytes.Length);
        Buffer.BlockCopy(lenbytes, 0, _buffers, 4, lenbytes.Length);
        Buffer.BlockCopy(bytes, 0, _buffers, 8, bytes.Length);
    }

    /// <summary>
    /// 消息Id
    /// </summary>
    public NetMessageId Id
    {
        get
        {
            return _msgId;
        }
    }

    /// <summary>
    /// Json格式数据
    /// </summary>
    public string Json
    {
        get
        {
            return Encoding.UTF8.GetString(this._body);
        }
    }

    /// <summary>
    /// 所有消息体字节数组，包括头和消息体
    /// </summary>
    public byte[] Bytes
    {
        get
        {
            return _buffers;
        }
    }
}
