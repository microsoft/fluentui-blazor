using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BcdLib.Components
{
    public enum PullDownStatus
    {
        /// <summary>
        /// 未开始
        /// </summary>
        Awaiting = 0,
        /// <summary>
        /// 已经下拉，未达到高度
        /// </summary>
        Pulling = 1,
        /// <summary>
        /// 达到高度，等待松手
        /// </summary>
        Loosing = 2,
        /// <summary>
        /// 已经松手，正在刷新
        /// </summary>
        Loading = 3, 
        /// <summary>
        /// 刷新完成
        /// </summary>
        Completed = 4, 
    }


    public enum PullUpStatus
    {
        /// 未开始
        /// </summary>
        Awaiting = 0,
        /// <summary>
        /// 已经下拉，未达到高度
        /// </summary>
        Pulling = 1,
        /// <summary>
        /// 达到高度，等待松手
        /// </summary>
        Loosing = 2,
        /// <summary>
        /// 已经松手，正在刷新
        /// </summary>
        Loading = 3,
        /// <summary>
        /// 刷新完成
        /// </summary>
        Completed = 4,
        /// <summary>
        /// 刷新完成, 但是没有更多的数据被加载
        /// </summary>
        NoData = 5,
    }
}
