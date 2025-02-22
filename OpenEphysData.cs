﻿using OpenCV.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSTH
{
    public enum DataType
    {
        Continuous,
        Spike,
        Event,
        Message
    }

    public interface IOpenEphysData
    {
        DataType Type { get; }
        //DateTimeOffset TimeStamp { get; }
        long MessageId { get; }
        long SampleNumber { get; }
        string Stream { get; }
    }

    public interface IContinuousData : IOpenEphysData
    {
        ushort Channel { get; }
        ushort SampleCount { get; }
        ushort SamplingRate { get; }
        Mat Data { get; }
    }

    public interface ISpikeData : IOpenEphysData
    {
        string Electrode { get; }
        byte NodeId { get; }
        ushort Channel { get; }
        ushort ChannelCount { get; }
        ushort SampleCount { get; }
        ushort SortedId { get; }
        Mat Data { get; }
    }

    public interface IEventData : IOpenEphysData
    {
        byte NodeId { get; }
        byte EventType { get; }
        byte EventLine { get; }
        byte EventState { get; }
        ulong EventWord { get; }
    }

    public interface IMessageData : IOpenEphysData
    {
        byte NodeId { get; }
        string Message { get; }
    }

    public class OpenEphysData : IContinuousData, ISpikeData, IEventData, IMessageData
    {
        public DataType Type { get; }
        //public DateTimeOffset TimeStamp { get; }
        public long MessageId { get; }
        public long SampleNumber { get; }
        public string Stream { get; }
        public string Electrode { get; }
        public byte NodeId { get; }
        public byte EventType { get; }
        public byte EventLine { get; }
        public byte EventState { get; }
        public ulong EventWord { get; }
        public ushort Channel { get; }
        public ushort ChannelCount { get; }
        public ushort SampleCount { get; }
        public ushort SamplingRate { get; }
        public ushort SortedId { get; }
        public string Message { get; }
        public Mat Data { get; }

        public OpenEphysData(long messageId, long sampleNumber, string stream, ushort channel,
            ushort sampleCount, ushort samplingRate, Mat data)
        {
            Type = DataType.Continuous;
            //TimeStamp = timeStamp;
            MessageId = messageId;
            SampleNumber = sampleNumber;
            Stream = stream;
            Channel = channel;
            SampleCount = sampleCount;
            SamplingRate = samplingRate;
            Data = data;
        }

        public OpenEphysData(long messageId, long sampleNumber, string stream,
            string electrode, byte nodeId, ushort channelCount, ushort sampleCount, ushort sortedId, Mat data)
        {
            Type = DataType.Spike;
            //TimeStamp = timeStamp;
            MessageId = messageId;
            SampleNumber = sampleNumber;
            Stream = stream;
            Electrode = electrode;
            NodeId = nodeId;
            ChannelCount = channelCount;
            SampleCount = sampleCount;
            SortedId = sortedId;
            Data = data;
        }

        public OpenEphysData(long messageId, long sampleNumber, string stream, byte nodeId,
            byte eventType, byte eventLine, byte eventState, ulong eventWord)
        {
            Type = DataType.Event;
            //TimeStamp = timeStamp;
            MessageId = messageId;
            SampleNumber = sampleNumber;
            Stream = stream;
            NodeId = nodeId;
            EventType = eventType;
            EventLine = eventLine;
            EventState = eventState;
            EventWord = eventWord;
        }

        public OpenEphysData(long messageId, long sampleNumber, string stream, byte nodeId, string message)
        {
            Type = DataType.Message;
            //TimeStamp = timeStamp;
            MessageId = messageId;
            SampleNumber = sampleNumber;
            Stream = stream;
            NodeId = nodeId;
            Message = message;
        }

        public override string ToString()
        {
            var header = $"[{MessageId}@{SampleNumber}]";
            switch (Type)
            {
                case DataType.Continuous:
                    return $"{header} LFP: channel {Channel}, {SampleCount} samples, {SamplingRate} Hz. ";
                case DataType.Spike:
                    return $"{header} Spike: {Electrode}, Id {SortedId}. ";
                case DataType.Event:
                    return $"{header} Event: line {EventLine}, {(EventState > 0 ? "HIGH" : "LOW")}. ";
                case DataType.Message:
                    return $"{header} Message: {Message}. ";
                default:
                    return base.ToString();
            }
        }
    }
}
