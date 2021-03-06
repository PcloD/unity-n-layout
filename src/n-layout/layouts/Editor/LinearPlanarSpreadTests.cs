#if N_LAYOUT_TESTS
using UnityEngine;
using NUnit.Framework;
using N;
using N.Package.Layout;
using N.Package.Layout.Layouts;
using N.Package.Animation;
using N.Package.Animation.Animations;
using N.Package.Animation.Curves;
using N.Package.Animation.Targets;
using N.Package.Core;
using N.Package.Core.Tests;

public class LinearPlanarSpreadTests : TestCase
{
  [Test]
  public void test_layout()
  {
    LayoutManagerTests.Reset(this);

    AnimationManager.Default.Configure(Streams.STREAM_0, AnimationStreamType.DEFER, 30);

    var layout = new LinearPlanarSpread(this.SpawnBlank());

    LayoutFactoryDelegate factory = (LayoutObject target) =>
    {
      var animation = new MoveSingle(target.position, target.rotation);
      animation.AnimationCurve = new Linear(1f);
      animation.AnimationTarget = new TargetSingle(target.gameObject);
      return animation;
    };

    var targets = new GameObject[]
    {
      this.SpawnBlank(),
      this.SpawnBlank(),
      this.SpawnBlank(),
      this.SpawnBlank(),
      this.SpawnBlank(),
      this.SpawnBlank(),
    };

    LayoutManager.Default.Add(Streams.STREAM_0, layout, factory, new TargetGroup(targets));

    Assert(AnimationManager.Default.Streams.Active(Streams.STREAM_0));

    int count = 0;
    AnimationManager.Default.Events.AddEventHandler<LayoutCompleteEvent>((ep) => { count += 1; });

    AnimationHandler.Default.Update(0.5f);

    Assert(AnimationManager.Default.Streams.Active(Streams.STREAM_0));
    Assert(AnimationManager.Default.Streams.Active(Streams.STREAM_0));
    Assert(count == 0);

    AnimationHandler.Default.Update(0.5f);
    AnimationHandler.Default.Update(0.5f);

    Assert(!AnimationManager.Default.Streams.Active(Streams.STREAM_0));
    Assert(count == 1);

    LayoutManagerTests.Reset(this);
  }
}
#endif