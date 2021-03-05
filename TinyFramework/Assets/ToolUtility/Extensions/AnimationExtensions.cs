using UnityEngine;

public static class AnimationExtensions
{
    public static void PlayDirectional(this Animator anim, string animationName, int layer = -1, float normalizedTime = 0f)
    {
        if (animationName.EndsWith("Left"))
        {
            animationName = animationName.Replace("Left", "Right");
        }
        anim.Play(animationName, layer, normalizedTime);
    }

    public static void PlayDirSimple(this Animator anim, string animName)
    {
        if (animName.EndsWith("Left"))
        {
            animName = animName.Replace("Left", "Right");
        }
        anim.Play(animName);
    }

    public static float GetAnimTime(this Animator anim)
    {
        return anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    public static bool AnimPlayed(this Animator anim, float normalizedTime = 1f)
    {
        return GetAnimTime(anim) >= normalizedTime;
    }

    public static AnimationClip GetAnimClip(this Animator anim, string name)
    {
        AnimationClip[] animationClips = anim.runtimeAnimatorController.animationClips;
        AnimationClip result = null;
        for (int i = 0; i < animationClips.Length; i++)
        {
            if (animationClips[i].name == name)
            {
                result = animationClips[i];
                break;
            }
        }
        return result;
    }

    public static void AdjustAnimSpeed(this Animator anim, string animName, float newAnimDuration = 2f)
    {
        AdjustAnimSpeed(anim, GetAnimClip(anim, animName), newAnimDuration);
    }

    public static void AdjustAnimSpeed(this Animator anim, AnimationClip animClip, float newAnimDuration = 2f)
    {
        anim.speed = GetAdjustedAnimSpeed(anim, animClip, newAnimDuration);
    }

    public static float GetAdjustedAnimSpeed(this Animator anim, AnimationClip animClip, float newAnimDuration = 2f)
    {
        if (newAnimDuration <= 0f)
        {
            return 9999f;
        }
        float length = animClip.length;
        float num = 1f;
        return length / newAnimDuration;
    }
}