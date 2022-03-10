using System;
using System.Collections.Generic;
using System.Text;
using ZXing.Mobile;

namespace Notes
{ 
    public interface IZXingHelper
    {
        //CameraResolutionSelectorDelegateImplementation
        CameraResolution SelectLowestResolutionMatchingDisplayAspectRatio(List<CameraResolution> availableResolutions);
    }
}
