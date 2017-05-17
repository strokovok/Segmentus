using Android.App;
using Android.Content;
using Android.Media;

namespace Segmentus
{
    static class SoundMaster
    {
        const int MaxStreams = 5;
        static SoundPool soundPool;

        static public int ButtonSound;
        static public int FailureSound;
        static public int PointBornSound;
        static public int PointChosenSound;
        static public int SegmentBornSound;
        static public int VictorySound;
        static public int SceneSwitchSound;

        static int lastStream = 0;
        static float volume;

        static SoundMaster()
        {
            soundPool = new SoundPool(MaxStreams, Stream.Music, 0);
            var prefs = Application.Context.GetSharedPreferences("AppPrefs", 
                FileCreationMode.Private);
            volume = prefs.GetFloat("volume", 1);
        }

        public static void SetVolume(float volume)
        {
            SoundMaster.volume = volume;
            var prefs = Application.Context.GetSharedPreferences("AppPrefs",
                FileCreationMode.Private);
            var editor = prefs.Edit();
            editor.PutFloat("volume", volume);
            editor.Commit();
            for (int i = 0; i < MaxStreams; ++i)
                soundPool.SetVolume(i, volume, volume);
        }

        public static void LoadSounds()
        {
            var c = Application.Context;
            ButtonSound = soundPool.Load(c, Resource.Raw.button, 0);
            FailureSound = soundPool.Load(c, Resource.Raw.failure, 0);
            PointBornSound = soundPool.Load(c, Resource.Raw.point_born, 0);
            PointChosenSound = soundPool.Load(c, Resource.Raw.point_chosen, 0);
            SegmentBornSound = soundPool.Load(c, Resource.Raw.segment_born, 0);
            VictorySound = soundPool.Load(c, Resource.Raw.victory, 0);
            SceneSwitchSound = soundPool.Load(c, Resource.Raw.scene_switch, 0);
        }

        public static void PlaySound(int soundID)
        {
            soundPool.Play(soundID, volume, volume, 0, 0, 1);
        }

        public static void StopAllSounds()
        {
            for (int i = 0; i < MaxStreams; ++i)
                soundPool.Stop(lastStream - i);
        }

        public static void UnloadSounds()
        {
            soundPool.Unload(ButtonSound);
            soundPool.Unload(FailureSound);
            soundPool.Unload(PointBornSound);
            soundPool.Unload(PointChosenSound);
            soundPool.Unload(SegmentBornSound);
            soundPool.Unload(VictorySound);
            soundPool.Unload(SceneSwitchSound);
        }

    }
}