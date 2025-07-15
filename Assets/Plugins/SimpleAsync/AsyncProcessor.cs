using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class AsyncProcessor : ITickable
{
    private readonly List<CoroutineInfo> _newWorkers = new();
    private readonly LinkedList<CoroutineInfo> _workers = new();

    public bool IsRunning => _workers.Any() || _newWorkers.Any();

    public void Tick()
    {
        AddNewWorkers();

        if (!_workers.Any())
        {
            return;
        }

        AdvanceFrameAll();
        AddNewWorkers();
    }

    public CoroutineInfo Process(IEnumerator process)
    {
        var data = new CoroutineInfo(process);

        _newWorkers.Add(data);

        return data;
    }
    
    public void Stop(CoroutineInfo worker)
    {
        if (worker == null)
        {
            return;
        }

        worker.IsFinished = true;
    }
    
    public void StopAll()
    {
        foreach (var worker in _workers)
        {
            worker.IsFinished = true;
        }

        foreach (var worker in _newWorkers)
        {
            worker.IsFinished = true;
        }

        _workers.Clear();
        _newWorkers.Clear();
    }

    private void AdvanceFrameAll()
    {
        var currentNode = _workers.First;

        while (currentNode != null)
        {
            var next = currentNode.Next;
            var worker = currentNode.Value;
            
            try
            {
                if (!worker.IsFinished)
                {
                    worker.CoRoutine.Pump();
                }

                worker.IsFinished = worker.CoRoutine.IsDone;
            }
            catch (Exception e)
            {
                worker.IsFinished = true;
                Debug.LogException(e);
            }

            if (worker.IsFinished)
            {
                _workers.Remove(currentNode);
            }

            currentNode = next;
        }
    }

    private void AddNewWorkers()
    {
        foreach (var worker in _newWorkers)
        {
            if (!worker.IsFinished)
            {
                _workers.AddLast(worker);
            }
        }
        
        _newWorkers.Clear();
    }
}

public class CoroutineInfo
{
    private static int _lastId;
        
    public  int Id { get; }
    public CoRoutine CoRoutine { get; }
    public bool IsFinished { get; set; }
    
    public CoroutineInfo(IEnumerator process)
    {
        Id = _lastId++;
        CoRoutine = new CoRoutine(process);
        IsFinished = false;
    }
}
