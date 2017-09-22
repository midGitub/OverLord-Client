using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class Tool
{
    public static int[] MergeIntArray(int[] nums1, int[] nums2)
    {
        int count1=nums1.Length;
        int count2=nums2.Length;
        int[] result=new int[count1+count2];
        nums1.CopyTo(result,0);
        nums2.CopyTo(result,count1);
        return result;
    }
}
