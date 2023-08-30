using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

internal class PoolSocketManager
{
    private static PoolSocketManager instance;

    public static PoolSocketManager Instance
    {
        get { return instance ??= new PoolSocketManager(); }
    }

    public void WinningIsCalled(int eventCode)
    {
        // It is called when the player 1 wins
        // code 18 if ball was striked correctly
        // code 19 if player 1 wins
        // code 20 if player poted 8 ball so opponent wins
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("PlayerNo", DataManager.Instance.playerNo);
        obj.AddField("EventCode", eventCode);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        TestSocketIO.Instace.Senddata("PlayerWon", obj);
    }
    
    public void PlayerBallType(int eventCode, bool value)
    {
        // 21 It is called when player selected ball
        // if value is true then player has solid ball and vis varsa
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("PlayerNo", DataManager.Instance.playerNo);
        obj.AddField("EventCode", eventCode);
        obj.AddField("BallType", value);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        TestSocketIO.Instace.Senddata("PlayerType", obj);
    }
    
    public void MovingBall(int eventCode)
    {
        // event code 14 for Opponent moving white ball before strike - show limits
        // event code 17 for Opponent moving white ball after strike - hide controllers
        // event code 16 for Opponent stoped moving white ball - hide limits
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("PlayerNo", DataManager.Instance.playerNo);
        obj.AddField("EventCode", eventCode);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        TestSocketIO.Instace.Senddata("WhiteBall", obj);
    }
    
    public void WhiteBallMovement(int eventCode, Vector3 angelPos)
    {
        //Opponent moving white ball
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("PlayerNo", DataManager.Instance.playerNo);
        obj.AddField("EventCode", eventCode);
        obj.AddField("SliderAngleX", angelPos.x);
        obj.AddField("SliderAngleY", angelPos.y);
        obj.AddField("SliderAngleZ", angelPos.z);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        TestSocketIO.Instace.Senddata("BallMovingAngel", obj);
    }
    
    public void ShortTurn(int eventCode,Vector3[] angelPos)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("PlayerNo", DataManager.Instance.playerNo);
        obj.AddField("EventCode", eventCode);
        obj.AddField("SliderAngleX", angelPos[0].x);
        obj.AddField("SliderAngleY", angelPos[0].y);
        obj.AddField("SliderAngleZ", angelPos[0].z);
        obj.AddField("TrickShotX", angelPos[1].x);
        obj.AddField("TrickShotY", angelPos[1].y);
        obj.AddField("TrickShotZ", angelPos[1].z);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        TestSocketIO.Instace.Senddata("ShortTurn", obj);
    }
    
    public void CueRotation(int eventCode, Quaternion rotation)
    {
        // Opponent rotated cue
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("PlayerNo", DataManager.Instance.playerNo);
        obj.AddField("EventCode", eventCode);
        obj.AddField("RotationX", rotation.x);
        obj.AddField("RotationY", rotation.y);
        obj.AddField("RotationZ", rotation.z);
        obj.AddField("RotationW", rotation.w);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        TestSocketIO.Instace.Senddata("CueRotation", obj);
    }
    
    public void CueSpinController(int eventCode, Vector3 angelPos)
    {
        //Opponent moving white ball
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("PlayerNo", DataManager.Instance.playerNo);
        obj.AddField("EventCode", eventCode);
        obj.AddField("SliderAngleX", angelPos.x);
        obj.AddField("SliderAngleY", angelPos.y);
        obj.AddField("SliderAngleZ", angelPos.z);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        TestSocketIO.Instace.Senddata("CueSpin", obj);
    }
    
    public void SpawnBalls(int eventCode, Vector3[] angelPos)
    {
        //Opponent moving white ball
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("PlayerNo", DataManager.Instance.playerNo);
        obj.AddField("EventCode", eventCode);
        obj.AddField("SliderAngleX", angelPos[0].x);
        obj.AddField("SliderAngleY", angelPos[1].y);
        obj.AddField("SliderAngleZ", angelPos[2].z);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        TestSocketIO.Instace.Senddata("SpawnBalls", obj);
    }
    
    public void SwitchUser(int eventCode, Vector3 angelPos)
    {
        //Opponent moving white ball
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("PlayerNo", DataManager.Instance.playerNo);
        obj.AddField("EventCode", eventCode);
        obj.AddField("SliderAngleX", angelPos.x);
        obj.AddField("SliderAngleY", angelPos.y);
        obj.AddField("SliderAngleZ", angelPos.z);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        TestSocketIO.Instace.Senddata("SwitchUser", obj);
    }
    
    public void CallPocket(int eventCode, int potNumber)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("PlayerNo", DataManager.Instance.playerNo);
        obj.AddField("EventCode", eventCode);
        obj.AddField("PotNumber", potNumber);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        TestSocketIO.Instace.Senddata("Pot", obj);
    }

    public void ShotPower(int eventCode, Vector3 angelPos)
    {
        //Opponent moving white ball
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("PlayerNo", DataManager.Instance.playerNo);
        obj.AddField("EventCode", eventCode);
        obj.AddField("SliderAngleX", angelPos.x);
        obj.AddField("SliderAngleY", angelPos.y);
        obj.AddField("SliderAngleZ", angelPos.z);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        TestSocketIO.Instace.Senddata("Power", obj);
    }
    
    
    public void TriggerBall(int eventCode, Vector3[] ballData)
    {
        //Opponent moving white ball
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("PlayerNo", DataManager.Instance.playerNo);
        obj.AddField("EventCode", eventCode);
        
        JSONObject ballArray = new JSONObject();

        foreach (Vector3 ballPosition in ballData)
        {
            JSONObject ballObject = new JSONObject();
            ballObject.AddField("x", ballPosition.x);
            ballObject.AddField("y", ballPosition.y);
            ballObject.AddField("z", ballPosition.z);
            ballArray.Add(ballObject);
        }
        obj.AddField("BallData", ballArray);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        TestSocketIO.Instace.Senddata("Trigger", obj);
    }
    
    public void SendMessage(int eventCode, string message)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("PlayerNo", DataManager.Instance.playerNo);
        obj.AddField("EventCode", eventCode);
        obj.AddField("Message", message);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        TestSocketIO.Instace.Senddata("Message", obj);
    }

}
