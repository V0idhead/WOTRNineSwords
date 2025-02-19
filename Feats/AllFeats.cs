﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoidHeadWOTRNineSwords.Feats
{
  static class AllFeats
  {
    public static void Configure()
    {
      ManeuverFocus.Configure();
      MythicManeuverFocus.Configure();
      UnnervingCalm.Configure();
      IronHeartAura.Configure();
      EnduranceOfStone.Configure();
      TigerBlooded.Configure();
      WhiteRavenDefense.Configure();
    }
  }
}